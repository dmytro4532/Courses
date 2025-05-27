export interface Test {
    id: string;
    title: string;
    description: string;
    questions: Question[];
}

export interface Question {
    id: string;
    content: string;
    answers: Answer[];
}

export interface Answer {
    id: string;
    value: string;
    isCorrect: boolean;
} 