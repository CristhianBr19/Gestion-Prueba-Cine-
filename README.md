# Sistema de Gestión de Cine - Prueba Técnica

Este proyecto es una solución Full Stack para la gestión de una cadena de cines, permitiendo administrar películas, salas y la asignación de funciones. [

---

##  Tecnologías y Versiones

### **Backend (API-REST)** 
* **Framework:** .NET Core 8.0 
* **ORM:** Entity Framework Core 
* **Base de Datos:** PostgreSQL 
* **Documentación:** Swagger 
* **Arquitectura:** Controllers, Model, Services y Repository 

### **Frontend** 
* **Framework:** Angular 17+
* **Framework CSS:** Bootstrap 5 
* **Estado:** Signals para reactividad eficiente.

---

##  Funcionalidades Implementadas

### **Procesos de Negocio (Backend)**
* **CRUD de Películas:** Gestión completa con eliminaciones lógicas[cite: 28, 38].
* **Búsqueda Avanzada:** Filtrado por nombre de película.
* **Validación de Fechas:** Filtro por fecha_publicacion con validación obligatoria.
* **Lógica de Salas:** Mensajes dinámicos según ocupación:
    * *"Sala disponible"* (< 3 películas).
    * "Sala con [n] películas asignadas"* (3 a 5 películas).
    * *"Sala no disponible"* (> 5 películas).

### **Interfaz de Usuario (Frontend)**
* **Login:** Acceso con datos por defecto.
* **Dashboard:** Indicadores de totales de salas, disponibilidad y películas.
* **Mantenimientos:** Pantallas para gestión de Películas y Sala.
* **Asignaciones:** Pantalla para vincular películas a salas.

---

##  Pruebas con Postman
Se incluye la colección de Postman en la carpeta `/Postman` del repositorio para validar todos los endpoints.

---
**Desarrollado por:** Cristhian Briones  
**Fecha:** Abril 2026
