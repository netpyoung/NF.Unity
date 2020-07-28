import os
import sys
import glob

from pathlib import Path

assets_path ='NextFramework/Assets'


def find_empty_directory_with_meta(base_dir):
    ret = []
    p = Path(assets_path)
    paths = list(p.glob("**/"))

    for path in paths:
        fpath = path.resolve()
        if os.listdir(fpath) == [] and Path(f"{fpath}.meta").exists():
            ret.append(fpath)
    return ret

def find_missing_origin(base_dir):
    ret = []
    p = Path(assets_path)
    paths = list(p.glob("**/*.meta"))

    for path in paths:
        meta_fpath = path.resolve()
        base_fpath, _ = os.path.splitext(meta_fpath)
        base_path = Path(base_fpath)
        if not base_path .exists():
            ret.append(path)
    return ret

def find_missing_meta(base_dir):
    ret = []
    p = Path(assets_path)
    paths = [f for f in p.glob('**/*.*') if f.suffix != '.meta']

    for path in paths:
        fpath = path.resolve()
        if not Path(f"{fpath}.meta").exists():
            ret.append(fpath)
    return ret



if __name__ == '__main__':
    empty_directory_with_meta = find_empty_directory_with_meta(assets_path)
    missing_origin = find_missing_origin(assets_path)
    missing_meta = find_missing_meta(assets_path)

    if len(empty_directory_with_meta) != 0:
        print("""
---------------- empty directory ----------------------
""")

        for path in find_empty_directory_with_meta(assets_path):
            print(path.resolve())
        exit(-1)


    if len(missing_origin) != 0:
        print("""
        ---------------- missing origin ----------------------
        """)
        for path in find_missing_origin(assets_path):
            print(path.resolve())
        exit(-1)

    if len(missing_meta) != 0:
        print("""
---------------- missing meta ----------------------
""")
        for path in find_missing_meta(assets_path):
            print(path.resolve())
        exit(-1)
