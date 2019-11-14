export interface iItem{
    Id: string;
    Name: string;
    Children?: iItem[];
    ParentId?: string;
}