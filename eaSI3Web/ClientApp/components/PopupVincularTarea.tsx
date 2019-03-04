import * as React from "react";
import { popupProps } from './Model/Props/popupProps';
import { VincularTarea } from "./VincularTarea";
import { PopupState } from './Model/States/PopupState';
import { RouteProps, RouteComponentProps } from 'react-router-dom';
export class PopupVincularTarea extends React.Component<popupProps, PopupState>{

    constructor(props: popupProps) {
        super(props);
        this.state = { idSi3:"" };
        this.vincular = this.vincular.bind(this);
    }

    public vincular(idSi3: string) {
        this.setState({ idSi3: idSi3 });
    }

    public render() {
        return (
            <div className="popup">
                <div className="popup_inner">
                    <button type="button" id="close" className="btn btn-danger btn-sm" onClick={() => { this.props.closePopup(this.state.idSi3) }}>X</button>
                    <h1 className="titlePopup">Vincular Tarea</h1>
                    <VincularTarea jiraKey={this.props.keyJira} vincular={this.vincular} />
                </div>
            </div>
        )
    }
}