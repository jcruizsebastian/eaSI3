import * as React from 'react';
import ReactLoading from "react-loading";
import { LoginGeneral } from './LoginGeneral';
import { LayoutState } from './Model/States/LayoutState';
import { NavMenu } from './NavMenu';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, LayoutState> {

    constructor(props: LayoutProps) {
        super(props);
        this.onLogin = this.onLogin.bind(this);
        this.getCookie = this.getCookie.bind(this);
        this.logout = this.logout.bind(this);
        this.validate = this.validate.bind(this);
        this.setCookie = this.setCookie.bind(this);

        this.state = { logged: false, cookiesOk: false, name: "", loaded: false };
    }
    async componentDidMount() {
        await this.validate();
    }

    //función para sacar las cookies, cname => userJira, passJira ... etc.
    public getCookie(cname: String) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    public validate() {

        var userJira = this.getCookie("userJira");
        var passJira = this.getCookie("passJira");
        var userSi3 = this.getCookie("userSi3");
        var passSi3 = this.getCookie("passSi3");
        var userId = this.getCookie("userId");
        this.setState({ loaded: false });

        fetch('api/Jira/validateLogin?', {
            method: 'post',
            body: JSON.stringify({ username: userJira, password: passJira, userId: userId }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => response.text() as Promise<String>)
            .then(data => {
                if (data.length == 0) {

                    fetch('api/Si3/validateLogin?', {
                        method: 'post',
                        body: JSON.stringify({ username: userSi3, password: passSi3, userId: userId }),
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                    })
                        .then(response => response.text() as Promise<String>)
                        .then(data => {
                            if (data.length == 0) { this.setState({ cookiesOk: true, loaded: true }); }
                            else { this.setState({ cookiesOk: false, loaded: true }); }
                        });
                } else {
                    this.setState({ cookiesOk: false, loaded: true });
                }
            });


    }

    //función para cambiar una cookie
    public setCookie(cname: string, cvalue: string, exdays: number) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    public onLogin(nameUser: string) {
        localStorage.removeItem('name');
        localStorage.setItem("name", nameUser);
        this.setState({ logged: true, cookiesOk: true });
    }

    public logout() {

        this.setCookie("userJira", "", 0);
        this.setCookie("passJira", "", 0);
        this.setCookie("userSi3", "", 0);
        this.setCookie("passSi3", "", 0);
        this.setCookie("codUserSi3", "", 0);

        this.setState({ logged: false });
    }

    public render() {

        var style = { backgroundColor: '#222', height: '50px' };
        let home;

        if (this.state.loaded) {
            if (document.cookie.length == 0) {
                home = <LoginGeneral onLogin={this.onLogin} />
            }
            else if (this.state.cookiesOk) {
                var name = localStorage.getItem('name');

                home = <div>
                    <div className='row'>
                        <div className='col-sm-12' style={style}>
                            <input type="button" className="btn btn-secondary" id="logout" value="Log out" onClick={this.logout} />
                            <label id="name">{name}</label>
                        </div>
                    </div>

                    <div className='row'>
                        <div className='col-sm-3'>
                            <NavMenu />
                        </div>
                        <div className='col-sm-9'>
                            {this.props.children}
                        </div>
                    </div>

                </div>
            } else { home = <LoginGeneral onLogin={this.onLogin} /> }
        }
        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>
        return <div className='container-fluid' >

            {home}

        </div>

    }
}
