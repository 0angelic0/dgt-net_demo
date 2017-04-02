let server = require('dgt-net').server
let packet = require('./packet')

let Room = require('./room')
let room = new Room()

////////////////////////////////////////////////////////////////////////////////
// Remote Proxy (Server Side)
////////////////////////////////////////////////////////////////////////////////

class RemoteProxy extends server.RemoteProxy {

  onConnected() {
    console.log("RemoteProxy There is a connection from " + this.getPeerName())
    room.addRemote(this)
  }

  onDisconnected() {
    console.log("RemoteProxy Disconnected from " + this.getPeerName())
    room.removeRemote(this)
  }

  login() {
    console.log('RemoteProxy login')
    this.send(packet.make_logged_in())
  }

  chat(msg) {
    console.log('RemoteProxy chat: ' + msg)
    room.broadcast(packet.make_chat(msg))
  }

  ping(pingTime) {
    console.log('RemoteProxy ping: ' + pingTime)
    this.send(packet.make_ping_success(pingTime))
  }

  float(f) {
    console.log('RemoteProxy float: ' + f)
    this.send(packet.make_float(f))
  }
}

module.exports = RemoteProxy