import { User } from '../User'

export interface LoginState {
    users: User[];
    userJiraLoaded: boolean;
    userSi3Loaded: boolean;
    loading: boolean;
}