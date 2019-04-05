# eaSI3

## TODO
* Completar manual con las anotaciones. 
* Que Jenkins pueda hacer los despliegues sin tener que meternos en srvsql05.
** Quizá probando msdeploy en vez de filesystem sea mejor.
** Para el problema del spinner en las opciones de publicar: https://developercommunity.visualstudio.com/content/problem/24972/publish-not-working-in-vs-2017-ide.html
* Emplear OAuth en vez de las credenciales de JIRA de cada usuario. (Por ahora no lo hacemos ya que necesitamos el usuario para sacar las tareas de la semana)
** Autenticación JIRA por OAUTH https://developer.atlassian.com/server/jira/platform/oauth/
* Añadir Logging de un modo elegante.
** Usar log en los módulos.
* Falta añadir la configuracion del connectionString en StatisticsContext.OnConfiguring.

## Beneficios
* ¿Hemos conseguido que la gente impute el viernes?
* ¿Cuantas horas estimamos que hemos ahorrado a la gente según las estadísticas en BBDD de eaSI3 viendo las tareas creadas y los partes de horas ingresados?