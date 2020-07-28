from invoke import task
import pathlib
import glob
import shutil
import git
import time
import os
import contextlib
import platform

# define global variable
repo = git.Repo('.', search_parent_directories=True)
NOW = time.strftime("%Y%m%d_%H%m%S")
ROOT = GIT_ROOT = pathlib.PurePath(repo.working_tree_dir).as_posix()

BUILD_ROOT_DIR = f"{ROOT}/__MESSAGE"
pathlib.Path(BUILD_ROOT_DIR).mkdir(parents=True, exist_ok=True)

MESSAGE_ROOT_DIR = os.path.abspath(f"{ROOT}/../wohapa_message/protos").replace('\\', '/')



if platform.system() == 'Darwin':
    os.environ['FrameworkPathOverride'] = '/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/4.6-api/'


def dotnet_run(args):
    cmd = f"dotnet run {args}"
    if platform.system() == 'Darwin':
        cmd = f"dotnet mono -f net46 -mo=\"--arch=32 --debug\" --loggerlevel Verbose -po=\"{args}\""
    print(cmd)
    return cmd


@contextlib.contextmanager
def chdir(dirname=None):
    #ref : https://github.com/pyinvoke/invoke/issues/225
    curdir = os.getcwd()
    try:
        if dirname is not None:
            os.chdir(dirname)
        yield
    finally:
        os.chdir(curdir)

@task()
def autogen_message(ctx):
    """generate interface message for unity_project"""

    output_dir = "tools/tool_message/AutoGenerated.Message/output"
    pathlib.Path(output_dir).mkdir(parents=True, exist_ok=True)
    ctx.run(f"protoc -I={MESSAGE_ROOT_DIR} -I={MESSAGE_ROOT_DIR}/common --csharp_out={output_dir} --csharp_opt=file_extension=.autogen.cs {MESSAGE_ROOT_DIR}/*.proto {MESSAGE_ROOT_DIR}/common/*.proto")

    with chdir('tools/tool_message'):
        with chdir('AutoGenerated.Message'):
            ctx.run('dotnet restore')
            ctx.run("dotnet build")

        with chdir('NF.Network.Protocol.Interface'):
            ctx.run('dotnet restore')
            ctx.run("dotnet build")


        dll = os.path.abspath('AutoGenerated.Message/bin/Debug/net46/AutoGenerated.Message.dll')
        interface_template = os.path.abspath("AutoGenerated.Interface/interface.liquid")
        interface_output = os.path.abspath("AutoGenerated.Interface/out")
        transfer_template = os.path.abspath("AutoGenerated.Transfer/transfer.liquid")
        transfer_output = os.path.abspath("AutoGenerated.Transfer/out")
        with chdir('NF.CLI.ProtocolGenerator'):
            ctx.run('dotnet restore')
            ctx.run(dotnet_run(f"--dll {dll} --interface_template {interface_template} --interface_output {interface_output} --transfer_template {transfer_template} --transfer_output {transfer_output}"))

        with chdir('NF.CLI.SendInfoGenerator'):
            meta = os.path.abspath(f"{ROOT}/../wohapa_message/meta.json")
            output = f"{ROOT}/DM/Assets/externals/AutoGenerated.Message/tools/DMMap.autogen.cs"
            template = os.path.abspath("template.liquid")
            ctx.run('dotnet restore')
            ctx.run(dotnet_run(f"--meta {meta} --output {output} --template {template}"))

        with chdir('AutoGenerated.Interface'):
            ctx.run('dotnet restore')
            ctx.run("dotnet build")

        with chdir('AutoGenerated.Transfer'):
            ctx.run('dotnet restore')
            ctx.run("dotnet build")


        for f in glob.glob(r'AutoGenerated.Transfer/bin/Debug/net46/*.dll'):
            shutil.copy(f, BUILD_ROOT_DIR)

    output = 'DM/Assets/externals/AutoGenerated.Message'
    pathlib.Path(output).mkdir(parents=True, exist_ok=True)

    # copy dll
    dlls = ['Google.Protobuf.dll', 'NF.Results.dll', 'NF.Network.Protocol.Interface.dll']
    for dll in dlls:
        shutil.copy(f"{BUILD_ROOT_DIR}/{dll}", output)

    # copy autogen
    cs_files = glob.glob(r"tools/tool_message/AutoGenerated.*/**Library.cs")
    cs_files = cs_files + glob.glob(r"tools/tool_message/AutoGenerated.*/**/*.autogen.cs")

    print(f"cs_files - {cs_files}")
    for cs in cs_files:
        dirname = f"{output}/{os.path.dirname(cs)}"
        print(f"{cs} -> {dirname}")
        pathlib.Path(dirname).mkdir(parents=True, exist_ok=True)
        shutil.copy(cs, dirname)

    print('done')


@task()
def test_protoc_server(ctx):
    """"""
    output_dir = "tools/tool_message/test_server"
    pathlib.Path(output_dir).mkdir(parents=True, exist_ok=True)
    ctx.run(f"protoc --proto_path={MESSAGE_ROOT_DIR} --python_out={output_dir} {MESSAGE_ROOT_DIR}/*.proto")
