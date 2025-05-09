# Gestor de Órdenes de Trabajo - Taller Automotriz
Este proyecto es una aplicación web que permite gestionar órdenes de reparación en un taller mecánico. Administra vehículos, órdenes de servicio, y asigna automáticamente al mecánico más adecuado según su especialidad, habilidades, experiencia y carga de trabajo.
## Funcionalidades principales
- Crear nuevas órdenes de reparación con selección de vehículo y tipo de reparación.
- Asignación automática de mecánicos basada en:
 - Especialidad requerida (mecánica, eléctrica, etc.)
 - Habilidades técnicas
 - Años de experiencia
 - Cantidad de órdenes activas (disponibilidad)
- Editar el estado de una orden (Pendiente, En Proceso, Finalizada, Entregada)
- Listar todas las órdenes y consultar su estado actual
- Registro completo del vehículo y relación con cliente
- Validación en back-end de datos sensibles como **Placa del Vehículo** (única y con formato válido)
- Relaciones lógicas entre entidades: Cliente → Vehículo → Orden → Mecánico
---
## Tecnologías utilizadas
- **Frontend**: Angular 16 (Standalone Components, Router, Forms)
- **Backend**: ASP.NET Core 7 Web API
- **Base de datos**: Azure SQL Database
- **ORM**: Entity Framework Core
- **Despliegue**: Render / Azure App Service
---
## Validaciones implementadas
### 1. Validación back-end de dato sensible:
- La **placa del vehículo** se valida en el servidor para evitar duplicados o formatos inválidos.  
 Esto asegura integridad sin depender del navegador del cliente.
### 2. Relación con dropdowns:
- Al registrar una orden, el vehículo no se ingresa manualmente.  
 Se selecciona desde un **dropdown poblado dinámicamente** desde la base de datos.
- De forma similar, el sistema **asigna automáticamente** al mecánico más adecuado, evitando que el usuario ingrese un ID manual.
---
## URL de despliegue
Accede a la app aquí:  

https://tallerauto-app.azurewebsites.net/

---
## Cómo ejecutar localmente
```bash
# Clona el repositorio
git clone [(https://github.com/mariabarrezueta/tallerauto.git)
# Backend
cd TallerAuto.Server
dotnet run
# Frontend
cd src/Web
npm install
ng serve
