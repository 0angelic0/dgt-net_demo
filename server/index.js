let server = require('dgt-net').server
let packet = require('./packet')
let RemoteProxy = require('./remoteproxy')

let port = 3456
server.setRemoteProxyClass(RemoteProxy)
server.setPacketObject(packet)
server.listen(port)