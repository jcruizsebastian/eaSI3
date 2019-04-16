import { WeekJiraIssues } from '../WeekJiraIssues';

export interface AgendaState {
    weekissues: WeekJiraIssues[];
    link: String;
    vincular: boolean;
    issueVincular: string;
    checked: boolean;
    expand: Number;
}