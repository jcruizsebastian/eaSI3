import { Product } from '../Product'

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
}