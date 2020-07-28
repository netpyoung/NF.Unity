import requests
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)


def hockey_distribute(service_token, app_id, output_path, comment='bugfix'):
    # ref: https://support.hockeyapp.net/kb/api/api-versions
    # ref: https://support.hockeyapp.net/discussions/problems/33355-is-uploadhockeyappnet-available-for-general-use
    # ref: http://stackoverflow.com/questions/10011810/http-post-form-in-ruby-with-custom-headers

    distribute_args = [
        "curl",
        '--insecure',
        '-F', '"status=2"',
        '-F', '"notify=0"',
        '-F', f'"notes={comment}"',
        '-F', f'"ipa=@\"{output_path}\""',
        '-H', f'"X-HockeyAppToken: {service_token}"',
        f'https://upload.hockeyapp.net/api/2/apps/{app_id}/app_versions/upload',
    ]

    print(" ".join(distribute_args))

    # requests
    headers = {
        'X-HockeyAppToken': service_token,
    }

    files = {
        'status': (None, '2'),
        'notify': (None, '0'),
        'notes': (None, comment),
        'ipa': (output_path, open(output_path, 'rb')),
    }
    response = requests.post(
        f'https://upload.hockeyapp.net/api/2/apps/{app_id}/app_versions/upload',
        headers=headers,
        files=files,
        verify=False
    )
    print(response)
