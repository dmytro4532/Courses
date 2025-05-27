import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { AttemptQuestion, CreateAttemptQuestionRequest } from '../types/test-attempts';
import axios from 'axios';

interface AttemptQuestionsState {
    questions: AttemptQuestion[];
    isLoading: boolean;
    error: string | null;
}

const initialState: AttemptQuestionsState = {
    questions: [],
    isLoading: false,
    error: null,
};

export const getAttemptQuestions = createAsyncThunk(
    'attemptQuestions/getQuestions',
    async (testAttemptId: string) => {
        const response = await axios.get<AttemptQuestion[]>(`/api/attemptquestions/testattempts/${testAttemptId}`);
        return response.data;
    }
);

export const createAttemptQuestion = createAsyncThunk(
    'attemptQuestions/create',
    async (request: CreateAttemptQuestionRequest) => {
        const response = await axios.post<AttemptQuestion>('/api/attemptquestions', request);
        return response.data;
    }
);

const attemptQuestionsSlice = createSlice({
    name: 'attemptQuestions',
    initialState,
    reducers: {
        clearQuestions: (state) => {
            state.questions = [];
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(getAttemptQuestions.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(getAttemptQuestions.fulfilled, (state, action) => {
                state.isLoading = false;
                state.questions = action.payload;
            })
            .addCase(getAttemptQuestions.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Failed to get attempt questions';
            })
            .addCase(createAttemptQuestion.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(createAttemptQuestion.fulfilled, (state, action) => {
                state.isLoading = false;
                const index = state.questions.findIndex(q => q.id === action.payload.id);
                if (index !== -1) {
                    state.questions[index] = action.payload;
                } else {
                    state.questions.push(action.payload);
                }
            })
            .addCase(createAttemptQuestion.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Failed to create attempt question';
            });
    },
});

export const { clearQuestions } = attemptQuestionsSlice.actions;
export default attemptQuestionsSlice.reducer; 