module.exports = {
  networks: {
      development: {
          host: "127.0.0.1",     // Dirección de Ganache
          port: 7545,            // Asegúrate de que este sea el puerto correcto
          network_id: "5777",       // Cualquier ID de red
      }
  },
  compilers: {
      solc: {
          version: "0.8.0",     // Asegúrate de usar una versión compatible
      }
  }
};
