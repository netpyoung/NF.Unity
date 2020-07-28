from invoke import task
import time
import git
import pathlib
import os
import subprocess
import tasks.util

# define global variable
repo = git.Repo('.', search_parent_directories=True)
NOW = time.strftime("%Y%m%d_%H%m%S")
ROOT = GIT_ROOT = pathlib.PurePath(repo.working_tree_dir).as_posix()

GIT_SHA = repo.head.object.hexsha

UNITY = 'C:/Program Files/Unity/Hub/Editor/2017.3.1f1/Editor/Unity.exe'

UNITY_PRJ_DIR = f"{ROOT}/NextFramework"
BUILD_ROOT_DIR = f"{ROOT}/BUILD"
BUILD_IPA_DIR = f"{BUILD_ROOT_DIR}/ipa"
UNITY_LOG_DIR = f"{BUILD_ROOT_DIR}/log"
UNITY_LOG_FPATH = f"{UNITY_LOG_DIR}/{NOW}.log"

# ensure directory.
pathlib.Path(BUILD_ROOT_DIR).mkdir(parents=True, exist_ok=True)
pathlib.Path(BUILD_IPA_DIR).mkdir(parents=True, exist_ok=True)
pathlib.Path(UNITY_LOG_DIR).mkdir(parents=True, exist_ok=True)

# unity methods
UNITY_BUILD_METHOD_AND = ''


# TODO(pyoung): need thread tail


@task(help={'country': "helloworld", 'stage': "stage"})
def build_and(ctx, country, stage):
    """test dock222 string"""
    print(NOW)
    print(GIT_ROOT)
    print(GIT_SHA)
    print(UNITY_PRJ_DIR)
    print(ROOT)
    print(UNITY_LOG_FPATH)

    build_number = os.getenv('BUILD_NUMBER', 1)
    output_path = f"{BUILD_ROOT_DIR}/{NOW}_{country}_{stage}.apk"
    print(f"OUTPUT PATH: {output_path}")


    ## start
    ## pre process
    ## register logger

    # UNITY_LOG_FPATH
    logger = tasks.util.FileTail(UNITY_LOG_FPATH)

    ## build apk using unity

    unity_args = [
        UNITY,
        "-quit",
        "-batchmode",
        "-nographics",
        "-buildTarget", "android",
        "-projectPath", f"{UNITY_PRJ_DIR}",
        "-logFile", f"{UNITY_LOG_FPATH}",
        "-executeMethod", f"{UNITY_BUILD_METHOD_AND}",
        f"-CustomArgs:output_path={output_path}@country={country}@stage={stage}"
    ]
    print(unity_args)
    p = subprocess.Popen(unity_args, stdout=subprocess.PIPE, shell=True)
    (output, err) = p.communicate()

    #This makes the wait possible
    p_status = p.wait()

    #This will give you the output of the command being executed
    print(f"Command output: {output}")

    logger.stop()
    ## check output_path exist
    ## distribute

    service_token = ''
    app_id = ''
    tasks.util.hockey_distribute(service_token, app_id, output_path)
    ## done
    print("done")
