import * as React from "react";
import { popupProps } from './Model/Props/popupProps';
import { VincularTarea } from "./VincularTarea";
import { RouteProps, RouteComponentProps } from 'react-router-dom';
export class PopupVincularTarea extends React.Component<popupProps, {}>{

    constructor(props: popupProps) {
        super(props);
    }

    public render() {
        return (
            <div className="popup">
                <div className="popup_inner">
                    <button type="button" id="close" className="btn btn-danger btn-sm" onClick={() => { this.props.closePopup() }}>X</button>
                    <h1 className="titlePopup">Vincular Tarea</h1>
                    <VincularTarea jiraKey={this.props.keyJira} />
                </div>
            </div>
        )
    }
}