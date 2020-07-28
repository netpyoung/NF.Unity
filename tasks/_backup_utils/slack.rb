# -*- coding: utf-8 -*-
require 'net/http'
require 'socket'
require 'json'
require 'slack'

def bot(message)
  chat_url = "http://localhost:9999/say"
  uri = URI(chat_url)
  params = {
    :message => message
  }
  begin
    Net::HTTP.post_form(uri, params)
  rescue
    puts "fail bot : #{uri} | #{params})"
  end
end

def bot_upload(channel, fpath)
  token = ''
  Slack.configure do |config|
    config.token = token
  end
  client = Slack::Web::Client.new(user_agent: 'Slack Ruby Client/1.0')
  client.files_upload(
    channels: channel,
    as_user: true,
    file: Faraday::UploadIO.new(fpath, 'application/octet-stream'),
	filename: File.basename(fpath)
  )
end
