# eaSI3

## TODO
* Completar manual con las anotaciones.
* Mergear master y trabajar sobre master y no sobre produccion.
* Emplear OAuth en vez de las credenciales de JIRA de cada usuario.
** Autenticación JIRA por OAUTH https://developer.atlassian.com/server/jira/platform/oauth/
* Que Jenkins pueda hacer los despliegues sin tener que meternos en srvsql05.
** Quizá probando msdeploy en vez de filesystem sea mejor.
** Para el problema del spinner en las opciones de publicar: https://developercommunity.visualstudio.com/content/problem/24972/publish-not-working-in-vs-2017-ide.html
* Encriptar las contraseñas en BBDD
** https://docs.microsoft.com/es-es/sql/relational-databases/security/encryption/encrypt-a-column-of-data?view=sql-server-2017
* Guardar campo de user Jira para guardar en BBDD.
* Añadir Logging de un modo elegante.
** Usar log en los módulos.
* Falta añadir la configuracion del connectionString en StatisticsContext.OnConfiguring.