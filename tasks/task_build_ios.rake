# -*- coding: utf-8 -*-
require "#{File.dirname(__FILE__)}/_defines.rb"
require "#{File.dirname(__FILE__)}/_os.rb"
require "#{File.dirname(__FILE__)}/_bot.rb"
require "#{File.dirname(__FILE__)}/_util.rb"
require "#{File.dirname(__FILE__)}/_hockeyapp.rb"



require 'rake'
require 'inifile'
require 'xcodeproj'



###################################
#
# BUILD_IOS
#
###################################
desc "BUILD_IOS"
# task :build_ios, [:COUNTRY, :STAGE, :MARKET] do |t, args|
#   country, stage, market = args.COUNTRY, args.MARKET, args.STAGE
#   output_fpath = "#{BUILD_ROOT_DIR}/#{NOW}_#{country}_#{market}_#{stage}.a
task :build_ios do
  build_number = ENV['BUILD_NUMBER']
  output_fpath = "#{BUILD_IOS_DIR}/#{NOW}"


  ## START


  # 기존에 Xcode가 떠 있다면, 킬시켜주고,
  sh "(kill $(ps aux | grep '[X]code' | awk '{print $2}')) | true"

  # Unity를 활용하여 XcodeProject를 생성한다.
  FileUtils.touch(UNITY_LOG_FPATH)
  logger = Logger.new(UNITY_LOG_FPATH, 10)

  build_cmd = [
    UNITY,
    "-quit -batchmode",
    "-nographics",
    "-buildTarget ios",
    "-projectPath #{UNITY_PRJ_DIR}",
    "-logFile #{UNITY_LOG_FPATH}",
    "-executeMethod #{UNITY_BUILD_METHOD_IOS}",
# "-CustomArgs:output_fpath=#{output_fpath}@country=#{country}@stage=#{stage}"
    "-CustomArgs:output_fpath=#{output_fpath}"
  ].join(' ');
  sh build_cmd do |ok, res|
    logger.stop()

    if not ok
      abort 'the operation failed'
    end
  end


  xcodeprj_fpath = "#{output_fpath}/Unity-iPhone.xcodeproj"
  app_name = 'helloworld'
  archive_fpath = "#{ENV['HOME']}/Library/Developer/Xcode/Archives/#{app_name}/Unity-iPhone-#{app_name}.xcarchive"
  # ipa_fpath = "#{BUILD_IOS_DIR}/#{NOW}_#{country}_#{market}_#{stage}.ipa"
  ipa_fpath = "#{BUILD_IOS_DIR}/#{NOW}.ipa"


  # 사용자 스킴 재생성.
  # ref: http://stackoverflow.com/a/20941812
  # 혹시 문제시 다음 방법 시도: http://stackoverflow.com/questions/5304031/where-does-xcode-4-store-scheme-data
  # prj = Xcodeproj::Project.open(xcodeprj_fpath)
  # prj.recreate_user_schemes()
  # prj.save()

  # xcarchive를 생성한다.
  sh "xcodebuild -project #{xcodeprj_fpath} -scheme Unity-iPhone archive -archivePath #{archive_fpath}"

  # ipa를 뽑아낸다.
  # sh "xcodebuild -exportArchive -archivePath #{archive_fpath} -exportPath #{ipa_fpath} -exportFormat ipa -exportProvisioningProfile #{PROVISIONING_NAME}"
  sh "xcodebuild -exportArchive -archivePath #{archive_fpath} -exportPath #{ipa_fpath} -exportFormat ipa"

  # TODO: http://qiita.com/roworks/items/7ef12acabf9679561d84

  ## distribute
  ini = IniFile.load("#{ENV['HOME']}/secret/hockeyapp.ini")
  app_token = ini['ios']['app_token']
  app_id = ini['ios']['app_id']
  hockey_distribute(app_token, app_id, ipa_fpath, GIT_HASH)

  ## TODO hockeyapp
  ## END
end
