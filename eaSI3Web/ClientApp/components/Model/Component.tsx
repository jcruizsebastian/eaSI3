import { Module } from './Module'

export interface Component {
    code: string;
    name: string;
    modulos: Module[];
}