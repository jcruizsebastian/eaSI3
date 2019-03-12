# eaSI3

## TODO
* Control peticiones SI3 (ActionBlock)
* Manual de uso easi3.
* Añadir Logging de un modo elegante.
* Para las tareas que no tengan id de si3, botón de asociar id.

* Ruta relativa en vez de absoulta en el LOG
* Falta añadir la configuracion del connectionString en StatisticsContext.OnConfiguring
* Pasar credenciales por body en métodos validate y usar un token para referenciar al usuario. (Usar userid como "token", meter passwords en BBDD)
** En caso de que el validatelogin (de acceso inicial a easi3) falle, borrar la cookie