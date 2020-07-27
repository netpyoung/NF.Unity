# -*- coding: utf-8 -*-
require "#{File.dirname(__FILE__)}/_defines.rb"
require "#{File.dirname(__FILE__)}/_os.rb"
require "#{File.dirname(__FILE__)}/_bot.rb"
require "#{File.dirname(__FILE__)}/_util.rb"
require "#{File.dirname(__FILE__)}/_hockeyapp.rb"


require 'rake'
require 'inifile'



###################################
#
# BUILD_AND
#
###################################
desc "build android"
# task :build_and, [:COUNTRY, :STAGE, :MARKET] do |t, args|
#   country, stage, market = args.COUNTRY, args.MARKET, args.STAGE
#   output_fpath = "#{BUILD_ROOT_DIR}/#{NOW}_#{country}_#{market}_#{stage}.apk"
task :build_and do
  build_number = ENV['BUILD_NUMBER']
  output_fpath = "#{BUILD_AND_DIR}/#{NOW}.apk"
  puts output_fpath
  ## START


  ## Unity를 활용하여 APK를 생성한다.
  FileUtils.touch(UNITY_LOG_FPATH)
  logger = Logger.new(UNITY_LOG_FPATH, 10)

  # ref: http://docs.unity3d.com/Manual/CommandLineArguments.html
  build_cmd = [
    UNITY,
    "-quit -batchmode",
    "-nographics",
    "-buildTarget android",
    "-projectPath #{UNITY_PRJ_DIR}",
    "-logFile #{UNITY_LOG_FPATH}",
    "-executeMethod #{UNITY_BUILD_METHOD_AND}",
    # "-CustomArgs:output_fpath=#{output_fpath}@country=#{country}@stage=#{stage}"
    "-CustomArgs:output_fpath=#{output_fpath}"
  ].join(' ')
  sh build_cmd do |ok, res|
    logger.stop()

    if not ok
      abort 'the operation failed'
    end
  end


  ## distribute
  ini = IniFile.load("#{ENV['HOME']}/secret/hockeyapp.ini")
  app_token = ini['and']['app_token']
  app_id = ini['and']['app_id']
  hockey_distribute(app_token, app_id, output_fpath, GIT_HASH)

  ## END
end
