import requests

INTERNAL_BASE_URL = ''


class DBUpdater(object):
    def __init__(self):
        self.api_get_version = f"{INTERNAL_BASE_URL}/version/get/client.db"
        self.api_update_version = f"{INTERNAL_BASE_URL}/version/set/client.db"

    def get_version(self):
        try:
            r = requests.get(self.api_get_version)
            print(f"r : {r} - {r.text}")
            json_data = r.json()
            curr_version = json_data['data']['version']
            return (curr_version, None)
        except Exception as e:
            return (None, e)

    def update_version(self, version, md5):
        try:
            data = {"version": version, "hash": md5}
            print(f"data : {data}")
            r = requests.put(self.api_update_version, json = data)
            print(r)
            print(r.text)
            print(r.json())
            return (r.json()['status'] == 'OK', None)
        except Exception as e:
            return (None, e)


class LocaleUpdater(object):
    def __init__(self):
        self.api_get_version = f"{INTERNAL_BASE_URL}/version/get/locale.db"
        self.api_update_version = f"{INTERNAL_BASE_URL}/version/set/locale.db"

    def get_version(self):
        try:
            r = requests.get(self.api_get_version)
            print(f"r : {r} - {r.text}")
            json_data = r.json()
            curr_version = json_data['data']['version']
            return (curr_version, None)
        except Exception as e:
            return (None, e)

    def update_version(self, version, md5):
        try:
            data = {"version": version, "hash": md5}
            print(f"data : {data}")
            r = requests.put(self.api_update_version, json = data)
            print(r)
            print(r.text)
            print(r.json())
            return (r.json()['status'] == 'OK', None)
        except Exception as e:
            return (None, e)
