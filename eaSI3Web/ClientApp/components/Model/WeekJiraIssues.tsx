import { JiraIssues } from './JiraIssues';

export interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}