# Despliegue de la Blockchain

## Requisitos Previos
- Tener instalado y abierto Ganache.
- Tener instalado Truffle en la PC.
  - Desde la consola: `npm install -g truffle` (este comando lo instala globalmente).

## Pasos para el Despliegue

1. Crear un nuevo workspace en Ganache (NO USAR QUICKSTART).
2. Configurar el archivo `truffle-config.js`:
    ```javascript
    host: "127.0.0.1",     // Dirección de Ganache
    port: 7545,            // Asegúrate de que este sea el puerto correcto
    network_id: "5777"     // ID que aparece en la parte superior de Ganache
    ```
3. Ir a la carpeta `SistemaDeVotacion.Contrato` y abrir la consola.
4. Ejecutar `truffle compile` desde la consola para compilar.
5. Ejecutar `truffle migrate --network development` desde la consola para desplegar en Ganache.
6. Buscar `Build/contracts/VotingSystem.json`, copiar el array de la propiedad `abi`, que se usará más adelante.

    -- Listo blockchain desplegada --

## Configuraciones Finales

### Configurar en `Program.cs` la dirección del contrato

### Configurar en Home Controller
```csharp
string privateKey = "0xcec7cc..";
```
clave privada del usuario

### Configurar Voting Service
private const string abi = "Aca va el abi entre comillas, pedir ayuda a ChatGPT para que lo meta acá";
