import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    user: string;
    pass: string;
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
}

interface WeekJiraIssues {
    fecha: string;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueId: string;
    titulo: string;
    tiempo: string;
}

export class Home extends React.Component<RouteComponentProps<{}>, UserCredentials> {



    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleChangePass = this.handleChangePass.bind(this);

        this.state = { user: '', pass: '', Weekissues: [], loadedJira: false, loadingJira: false };
    }

    public handleChangeUser(event: { target: { value: any; }; }) {

        this.setState({ user: event.target.value });
    }

    public handleChangePass(event: { target: { value: any; }; }) {

        this.setState({ pass: event.target.value });
    }

    public handleSubmit(e: { preventDefault: () => void; }) {

        e.preventDefault();

        this.setState({ loadingJira: true });

        fetch('api/Jira/worklog?username=' + this.state.user + '&password=' + this.state.pass)
            .then(response => response.json() as Promise<WeekJiraIssues[]>)
            .then(data => {
                this.setState({ Weekissues: data, loadingJira: false, loadedJira: true  });
            });
    }

    private static renderAgenda(Weekissues: WeekJiraIssues[]) {
        
        return <div>
            <table className="table table-condensed table-bordered">
            <thead>
                <tr>
                    <th>Fecha</th>
                    <th>Tareas</th>
                    <th>Horas</th>
                </tr>
            </thead>
            <tbody>
                {
                    Weekissues.map(Weekissue => 

                        <tr>
                            <td className="agenda-date active">
                                <div className="shortdate text-muted">{Weekissue.fecha}</div>
                            </td>
                            <td className="agenda-events">
                                {Weekissue.issues.map(issue => <div> {issue.titulo} </div>)}
                        </td>
                            <td className="agenda-events">
                                {Weekissue.issues.map(issue => <div> {issue.tiempo} </div>)}
                            </td>
                        </tr>

                        )
                }
            </tbody>
            </table>
        </div>
    }

    public render() {
        let agenda = <p><em>Sin datos</em></p>;

        if (this.state.loadedJira)
            agenda = Home.renderAgenda(this.state.Weekissues);

        if (this.state.loadingJira)
            agenda = <ReactLoading color='#000' type='balls' />

        return <div>
            
            <form className="commentForm" onSubmit={this.handleSubmit}>
                <input type="text" id="tbUser" name="tbUser" value={this.state.user} onChange={this.handleChangeUser} placeholder='jiraUser' />
                <input type="password" id="tbPass" name="tbPass" value={this.state.pass} onChange={this.handleChangePass} placeholder='jiraPass' />

                <input disabled={this.state.loadingJira} type="submit" value="Post" />
            </form>

            {agenda}

            </div>
    }
}