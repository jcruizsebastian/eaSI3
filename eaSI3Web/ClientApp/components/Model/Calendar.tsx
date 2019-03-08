import { CalendarWeeks } from './CalendarWeeks'

export interface Calendar {
    version: string;
    weeks: CalendarWeeks[];
}