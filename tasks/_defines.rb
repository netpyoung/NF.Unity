# -*- coding: utf-8 -*-
require "#{File.dirname(__FILE__)}/_util.rb"
require "#{File.dirname(__FILE__)}/_os.rb"

require 'date'
require 'fileutils'

#####################
# Common
#####################
NOW = Time.now.strftime("%Y%m%d_%H%M%S")
GIT_ROOT = `git rev-parse --show-toplevel`.strip
GIT_HASH = `git rev-parse HEAD`.strip
ROOT_DIR = GIT_ROOT


#####################
# Unity
#####################
UNITY = which('unity')
UNITY_PRJ_DIR = "#{ROOT_DIR}/unity_project"
BUILD_ROOT_DIR = "#{ROOT_DIR}/BUILD"
BUILD_IOS_DIR = "#{ROOT_DIR}/BUILD/IOS"
BUILD_AND_DIR = "#{ROOT_DIR}/BUILD/AND"
BUILD_LOG_DIR = "#{ROOT_DIR}/BUILD/log"
UNITY_LOG_FPATH = "#{BUILD_ROOT_DIR}/log/#{NOW}.log"

FileUtils.mkdir BUILD_ROOT_DIR unless File.exist?(BUILD_ROOT_DIR)
FileUtils.mkdir BUILD_IOS_DIR  unless File.exist?(BUILD_IOS_DIR)
FileUtils.mkdir BUILD_AND_DIR  unless File.exist?(BUILD_AND_DIR)
FileUtils.mkdir BUILD_LOG_DIR  unless File.exist?(BUILD_LOG_DIR)


UNITY_BUILD_METHOD_AND = 'skeleton.Editor.Batch.ExecuteMethods.BuildAnd'
UNITY_BUILD_METHOD_IOS = 'skeleton.Editor.Batch.ExecuteMethods.BuildIOS'
UNITY_BUILD_METHOD_BUNDLE = 'skeleton.Editor.Batch.ExecuteMethods.BuildBundle'


#####################
# Unity - iOS
#####################
PROVISIONING_NAME = "Enterprise" # TODO: country, stage, market별 분기 처리.





#####################
# Jenkins
#####################
JENKINS_ROOT_URL = 'http://JENKINS_URL:8080'
JENKINS_JOB_AND = "#{JENKINS_ROOT_URL}/job/BUILD_AND"
JENKINS_JOB_IOS = "#{JENKINS_ROOT_URL}/job/BUILD_IOS"


#####################
# Etc
#####################

DRIP_MESSAGE = <<EOS
(ノಠ益ಠ)ノ ┻━┻
EOS


## ref: http://dobiho.com/?p=7192
# 용량 부족시 정리.
#  Xcode > Preference > Locations
# ~/Library/Developer/Xcode/DerivedData
# ~/Library/Developer/Xcode/Archives
# ~/Library/Developer/Xcode/iOS Device Logs
# ~/Library/Developer/Xcode/ iOS DeviceSupport
# https://get.slack.help/hc/en-us/articles/206870177-Creating-custom-emoji
# https://get.slack.help/hc/en-us/articles/202931348-Emoji-and-emoticons
