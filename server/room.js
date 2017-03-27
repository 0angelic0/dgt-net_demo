class Room {
  constructor() {
    this.remotes = []
  }

  addRemote(remote) {
    this.remotes.push(remote)
  }

  removeRemote(remote) {
    this.remotes.splice(this.remotes.indexOf(remote), 1)
  }

  broadcast(data) {
    this.remotes.forEach((remote) => {
      remote.send(data)
    })
  }

  broadcastExcept(exceptRemote, data) {
    this.remotes.forEach((remote) => {
      if (remote == exceptRemote) return
      remote.send(data)
    })
  }
}

module.exports = Room