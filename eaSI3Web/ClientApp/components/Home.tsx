import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import LoginJira from './LoginJira';

//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    user: string;
    pass: string;
    userSI3: string;
    passSI3: string;
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
    loadedSI3: boolean;
    loadingSI3: boolean;
}

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueId: string;
    titulo: string;
    tiempo: string;
    tiempoCorregido: string;
}

export class Home extends React.Component<RouteComponentProps<{}>, UserCredentials> {

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.handleSubmitSi3 = this.handleSubmitSi3.bind(this);
        this.handleChangeUserSI3 = this.handleChangeUserSI3.bind(this);
        this.handleChangePassSI3 = this.handleChangePassSI3.bind(this);
        

        this.state = { user: 'jcruiz', pass: '_*_d1d4ct1c97', Weekissues: [], loadedJira: false, loadingJira: false, passSI3: '', userSI3: '', loadedSI3: false, loadingSI3: false };
    }

    public handleChangeUserSI3(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ userSI3: event.currentTarget.value });
    }

    public handleChangePassSI3(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ passSI3: event.currentTarget.value });
    }
    /*
    public prueba(event: React.FormEvent<HTMLInputElement>) {

        let day = event.currentTarget.id.split('-')[1];
        let issueId = event.currentTarget.id.split('-')[0];

        for (let dayIssue of this.state.Weekissues)
        {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day)
            {
                for (let issue of dayIssue.issues) {
                    if (issue.issueId == issueId)
                        issue.tiempo = event.currentTarget.value;
                }
            }
        }
    }
    */
    public handleSubmitSi3(e: { preventDefault: () => void; }) {

        e.preventDefault();

        this.setState({ loadingSI3: true });

        fetch('api/SI3/register?username=' + this.state.userSI3 + '&password=' + this.state.passSI3, {
            method: 'post',
            body: JSON.stringify(this.state.Weekissues),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(res => {
                console.log(res);
            });
    }

    /*
    public handleSubmit(e: { preventDefault: () => void; }) {

        //handleSubmitJira(e);
        
        e.preventDefault();

        this.setState({ loadingJira: true });

        fetch('api/Jira/worklog?username=' + this.state.user + '&password=' + this.state.pass)
            .then(response => response.json() as Promise<WeekJiraIssues[]>)
            .then(data => {
                this.setState({ Weekissues: data, loadingJira: false, loadedJira: true });
            });
       
    }
    */
    public onJiraDataLoaded(weekJiraIssues: WeekJiraIssues[])
    {
         this.setState({ loadedJira: true, Weekissues: weekJiraIssues }); 
    }

    private renderAgenda(Weekissues: WeekJiraIssues[]) {

        return <Agenda weekissues={Weekissues} />

            /*
            <form className="si3Form" onSubmit={this.handleSubmitSi3}>
                <input type="text" id="tbUserSI3" name="tbUserSI3" value={this.state.userSI3} onChange={this.handleChangeUserSI3} placeholder='si3 user' />
                <input type="password" id="tbPassSI3" name="tbPassSI3" value={this.state.passSI3} onChange={this.handleChangePassSI3} placeholder='si3 pass' />
                <input type="submit" value="Enviar a SI3" />
            </form>
            */

        
    }

    public render() {

        let agenda = <p><em>Sin datos</em></p>;

        if (this.state.loadedJira)
            agenda = this.renderAgenda(this.state.Weekissues);

        /*
        if (!this.state.loadedJira)
            agenda = <ReactLoading color='#000' type='balls' />
        */
        

        
        
        return (
            <div>
            <LoginJira onJiraLoginSuccessfully={this.onJiraDataLoaded} />
            {agenda} 

            </div>
        )
            
            /*
            <div>
            
            <form className="dataForm" onSubmit={this.handleSubmit}>
                <input type="text" id="tbUser" name="tbUser" value={this.state.user} onChange={this.handleChangeUser} placeholder='jira user name' />
                <input type="password" id="tbPass" name="tbPass" value={this.state.pass} onChange={this.handleChangePass} placeholder='jira pass' />

                <input disabled={this.state.loadingJira} type="submit" value="Obtener Issues de Jira" />
            </form>

            {agenda}

            </div>
            */
                
        
    }
}
export default Home


