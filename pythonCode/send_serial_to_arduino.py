# -*- coding: utf-8 -*-
"""
Created on Fri Sep 11 09:16:28 2015

@author: Itamark
"""

import serial
import struct

def send_msg(id, r, g, b):
    ser = serial.Serial('COM6')    
    ser.write(struct.pack('BBBB', ord(id), r, g, b))
    ser.close()


def send_msg2(id, bytes):
    msg = [ord(id)] + bytes    
    str = struct.pack('!{0}B'.format(len(msg)), *msg)    
    ser = serial.Serial('COM6')        
    ser.write(str)
    ser.close()
# DEBUG
#bytes = (128,255,0)
#str = struct.pack('!{0}B'.format(len(bytes)), *bytes)
#str = '\xaa\xff\x00'
#ser.write(str)
#ser.close()