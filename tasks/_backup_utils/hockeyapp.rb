# -*- coding: utf-8 -*-
def hockey_distribute(service_token, app_id, output_path, comment='bugfix')
  ## output을 hockeyapp에 배포한다.
  # ref: https://support.hockeyapp.net/kb/api/api-versions
  # ref: https://support.hockeyapp.net/discussions/problems/33355-is-uploadhockeyappnet-available-for-general-use
  # ref: http://stackoverflow.com/questions/10011810/http-post-form-in-ruby-with-custom-headers

  cmd = [
    "curl",
#    ' --insecure',
    '-F "status=2"',
    '-F "notify=0"', 
    %Q{-F "notes=#{comment}"},
    %Q{-F "ipa=@#{output_path}"},
    %Q{-H "X-HockeyAppToken: #{service_token}"},
    'https://upload.hockeyapp.net/api/2/apps/' + app_id + '/app_versions/upload'
  ].join(' ')
  puts cmd
  system(cmd)
end
