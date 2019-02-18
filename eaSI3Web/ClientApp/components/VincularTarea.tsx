import * as React from "react";
import { RouteComponentProps } from "react-router";
import '../css/vincularTarea.css';

export class VincularTarea extends React.Component<RouteComponentProps<{}>, {}> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);
    }
    public handleSubmit() { }
    public render() {
        return (
        <div>
            <label className="text">Key Jira :</label>
                <input type="text" id="keyJira" className="form-control" name="key" ref="tbKeyJira" placeholder="Introduzca Key Jira" autoComplete="off" />
                <input type="submit" className="btn btn-primary" value="Generar información" onSubmit={this.handleSubmit} />
        </div>
        )
    }
}