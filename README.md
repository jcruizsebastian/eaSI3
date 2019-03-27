# eaSI3

## TODO
* Componer el titulo de las tareas en SI3 como "IDJIRA - titulo de la tarea en jira" (actualmente se visualiza solo el IDJIRA), ejemplo "COR-132 - Revisión gráfico evolución"
* Actualmente el campo de ID SI3 en JIRA, soporta tareas y proyectos o nada, si es nada pide vincular. Queremos que pida vincular también si acaba en ;. 
		Es decir, la tarea con ID SI3: 4515;54821;87878 actualmente coge como id de si3 87878, si tenemos algo como 4515;54821;87878; entonces pedirá vincular de nuevo.

* Bug cuando se cambian las contraseñas de Jira.
* Indicar en el manual que las vacaciones hay que poner la del padre.
* Guardar campo de usaer Jira para guardar en BBDD.
* Añadir Logging de un modo elegante.
** Usar log en los módulos.
* Falta añadir la configuracion del connectionString en StatisticsContext.OnConfiguring.
* Validación/imputación de horas en hilos (con un pool de hilos para no saturar el servidor).