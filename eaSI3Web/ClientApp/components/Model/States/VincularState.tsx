import { Product } from '../Product'
import { Project } from '../Project';
import { Milestones } from '../Milestones';

export interface VincularState {
    products: Product[]
    productSelected: string;
    componentSelected: string;
    moduleSelected: string;
    loadedData: boolean;
    loadedDataJira: boolean;
    titulo: string;
    prioridad: string;
    tipo: string;
    responsable: string;
    loading: boolean;
    todoOk: boolean;
    projects: Project[];
    projectSelected: string;
    milestones: Milestones[];
    milestoneSelected: string;
}