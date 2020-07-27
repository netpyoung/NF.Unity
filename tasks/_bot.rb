# -*- coding: utf-8 -*-
require 'net/http'
require 'socket'
require 'json'


def bot(message)
  chat_url = "http://localhost:8888/hubot/say"
  uri = URI(chat_url)
  params = {
    :message => message
  }
  begin
    Net::HTTP.post_form(uri, params)
  rescue
    puts "fail bot : #{uri} | #{params}}"
  end
end
