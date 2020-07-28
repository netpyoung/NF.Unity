from boto3.session import Session
import json
import hashlib
import requests



ACCESS_KEY=''
SECRET_KEY=''


def upload_db_to_aws(fpath, version):
    try:
        file_bytes = open(fpath, "rb").read()
        m = hashlib.md5()
        m.update(file_bytes)
        md5 = m.hexdigest().upper()

        json_txt = json.dumps({"version": version, "md5": md5})

        print(json_txt)

        with open('__DB/LATEST_DB.txt', 'w') as f:
            f.write(json_txt)


        session = Session(aws_access_key_id=ACCESS_KEY,
                      aws_secret_access_key=SECRET_KEY)
        s3 = session.resource('s3')

        bucket = s3.Bucket(BUCKET_NAME)

        bucket.upload_file(
            fpath,
            f"game_db/{version}/client.db",
            ExtraArgs={'ACL':'public-read'}
        )

        bucket.upload_file(
        '__DB/LATEST_DB.txt',
            f"game_db/{version}/LATEST_DB.txt",
            ExtraArgs={'ACL':'public-read'}
        )

        bucket.upload_file(
            '__DB/LATEST_DB.txt',
            f"game_db/LATEST_DB.txt",
            ExtraArgs={'ACL':'public-read'}
        )

        data = {"version": version, "hash": md5}
        return (data, None)
    except Exception as e:
        return (None, e)




def upload_locale_to_aws(fpath, version):
    try:
        file_bytes = open(fpath, "rb").read()
        m = hashlib.md5()
        m.update(file_bytes)
        md5 = m.hexdigest().upper()

        json_txt = json.dumps({"version": version, "md5": md5})

        print(json_txt)

        with open('__DB/LATEST_LOCALE.txt', 'w') as f:
            f.write(json_txt)

        session = Session(aws_access_key_id=ACCESS_KEY,
                          aws_secret_access_key=SECRET_KEY)
        s3 = session.resource('s3')
        bucket = s3.Bucket(BUCKET_NAME)
        bucket.upload_file(
            fpath,
            f"locale_db/{version}/locale.db",
            ExtraArgs={'ACL':'public-read'}
        )
        bucket.upload_file(
            '__DB/LATEST_LOCALE.txt',
            f"locale_db/{version}/LATEST_LOCALE.txt",
            ExtraArgs={'ACL':'public-read'}
        )
        bucket.upload_file(
        '__DB/LATEST_LOCALE.txt',
            f"locale_db/LATEST_LOCALE.txt",
            ExtraArgs={'ACL':'public-read'}
        )

        data = {"version": version, "hash": md5}
        return (data, None)
    except Exception as e:
        return (None, e)
