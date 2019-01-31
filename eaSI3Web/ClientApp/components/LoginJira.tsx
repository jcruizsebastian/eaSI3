import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import Home from './Home';
import onJiraDataLoaded from './Home';

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueSi3: string;
    issueCode: string;
    issueKey: string;
    titulo: string;
    tiempo: number;
    tiempoCorregido: number;
}

interface UserCredentials {
    user: string;
    pass: string;
    Weekissues: WeekJiraIssues[];
    loadingJira: boolean;
}

interface LoginJiraProps {
    onJiraLoginSuccessfully: Function;
}

export class LoginJira extends React.Component<LoginJiraProps, UserCredentials> {

    constructor(props: LoginJiraProps) {
        super(props);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleChangePass = this.handleChangePass.bind(this);
        this.confirmLoadedJira = this.confirmLoadedJira.bind(this);

        this.state = { user: 'jcruiz', pass: '_*_d1d4ct1c97', Weekissues: [], loadingJira: false};


    }

    public handleSubmit(e: { preventDefault: () => void; }) {

        //handleSubmitJira(e);

        e.preventDefault();

        this.setState({ loadingJira: true });

        fetch('api/Jira/worklog?username=' + this.state.user + '&password=' + this.state.pass)
            .then(response => response.json() as Promise<WeekJiraIssues[]>)
            .then(data => {
                this.setState({ Weekissues: data}, this.confirmLoadedJira);
            });

    }

    public confirmLoadedJira() {
 
        console.log("Prueba")
        this.props.onJiraLoginSuccessfully(this.state.Weekissues);

    }

    public handleChangeUser(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ user: event.currentTarget.value });
    }

    public handleChangePass(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ pass: event.currentTarget.value });
    }


    public render() {

        return <div>

            <form className="dataForm" onSubmit={this.handleSubmit}>
                <input type="text" id="tbUser" name="tbUser" value={this.state.user} onChange={this.handleChangeUser} placeholder='jira user name' />
                <input type="password" id="tbPass" name="tbPass" value={this.state.pass} onChange={this.handleChangePass} placeholder='jira pass' />
                <input disabled={this.state.loadingJira} type="submit" value="Obtener Issues de Jira" />
            </form>


        </div>


    }
}
export default LoginJira
