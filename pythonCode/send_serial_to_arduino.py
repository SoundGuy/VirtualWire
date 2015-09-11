# -*- coding: utf-8 -*-
"""
Created on Fri Sep 11 09:16:28 2015

@author: Itamark
"""

import serial
import struct
import time

def send_msg(id, r, g, b):
    ser = serial.Serial('COM6')    
    ser.write(struct.pack('BBBB', ord(id), r, g, b))
    ser.close()


def send_msg2(id, bytes):
    wait_time = 1
    msg = [ord(id)] + bytes    
    str = struct.pack('!{0}B'.format(len(msg)), *msg)    
    ser = serial.Serial('COM6')        
    ser.write(str)
    ser.close()
    time.sleep(wait_time) # let the msg time to finish

#--- create dictionary of commands to record
# colors on
send_msg2('C',[0,0,0])
send_msg2('C',[0,0,255])
send_msg2('C',[0,255,0])
send_msg2('C',[0,255,255])
send_msg2('C',[255,0,0])
send_msg2('C',[255,0,255])
send_msg2('C',[255,255,0])
send_msg2('C',[255,255,255])
# colors fade
for fade_time in [16, 50, 150, 255]:
    send_msg2('c',[0,0,255,fade_time])
    send_msg2('c',[0,255,0,fade_time])
    send_msg2('c',[0,255,255,fade_time])
    send_msg2('c',[255,0,0,fade_time])
    send_msg2('c',[255,0,255,fade_time])
    send_msg2('c',[255,255,0,fade_time])
    send_msg2('c',[255,255,255,fade_time])