import { z } from "zod";

export default interface TodoItem {
    id: number,
    name: string,
    isComplete: boolean
}

export const TodoSchema = z.object({
    id: z.number().nonnegative(),
    name: z.string().max(100),
    isComplete: z.boolean()
})