import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { AttemptQuestion, PagedList } from '../../types';

interface AttemptQuestionsState {
    questions: PagedList<AttemptQuestion>;
    isLoading: boolean;
    error: string | null;
}

const initialState: AttemptQuestionsState = {
    questions: {
        pageIndex: 1,
        pageSize: 10,
        totalCount: 0,
        hasPreviousPage: false,
        hasNextPage: false,
        items: []
    },
    isLoading: false,
    error: null,
};

export const getAttemptQuestions = createAsyncThunk(
    'attemptQuestions/getQuestions',
    async (testAttemptId: string, { rejectWithValue }) => {
        try {
            const response = await api.get<PagedList<AttemptQuestion>>(`/api/attemptquestions/testattempts/${testAttemptId}`);
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to get attempt questions');
        }
    }
);

export const createAttemptQuestion = createAsyncThunk(
    'attemptQuestions/create',
    async (request: { testAttemptId: string; questionId: string; selectedAnswerIds: string[] }, { rejectWithValue }) => {
        try {
            const response = await api.post<AttemptQuestion>('/api/attemptquestions', request);
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to create attempt question');
        }
    }
);

const attemptQuestionsSlice = createSlice({
    name: 'attemptQuestions',
    initialState,
    reducers: {
        clearQuestions: (state) => {
            state.questions = {
                pageIndex: 1,
                pageSize: 10,
                totalCount: 0,
                hasPreviousPage: false,
                hasNextPage: false,
                items: []
            };
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
                state.error = action.payload as string;
            })
            .addCase(createAttemptQuestion.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(createAttemptQuestion.fulfilled, (state, action) => {
                state.isLoading = false;
                state.questions.items = state.questions.items.map((q: AttemptQuestion) => 
                    q.questionId === action.payload.questionId ? action.payload : q
                );
            })
            .addCase(createAttemptQuestion.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload as string;
            });
    },
});

export const { clearQuestions } = attemptQuestionsSlice.actions;
export default attemptQuestionsSlice.reducer; 