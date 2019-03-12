# eaSI3

## TODO
* Codificar contraseñas.
* Añadir Logging de un modo elegante.

* Ruta relativa en vez de absoulta en el LOG
* Falta añadir la configuracion del connectionString en StatisticsContext.OnConfiguring
* Usar log en los módulos
* Pasar credenciales por body en métodos validate y usar un token para referenciar al usuario. (Usar userid como "token", meter passwords en BBDD)
** En caso de que el validatelogin (de acceso inicial a easi3) falle, borrar la cookie