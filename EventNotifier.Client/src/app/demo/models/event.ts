import { Category } from "./category";
import { Format } from "./format";

export interface Event {
    id?: number;
    name?: string;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    year?: number;
    format?: Format;
    category?: Category;
    photo?: Uint8Array[];
}
