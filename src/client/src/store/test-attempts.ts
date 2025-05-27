import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { TestAttempt } from '../types/test-attempts';
import axios from 'axios';

interface TestAttemptsState {
    activeAttempt: TestAttempt | null;
    currentAttempt: TestAttempt | null;
    isLoading: boolean;
    error: string | null;
}

const initialState: TestAttemptsState = {
    activeAttempt: null,
    currentAttempt: null,
    isLoading: false,
    error: null,
};

export const getActiveTestAttempt = createAsyncThunk(
    'testAttempts/getActive',
    async (testId: string) => {
        const response = await axios.get<TestAttempt>(`/api/testattempts/active/${testId}`);
        return response.data;
    }
);

export const getTestAttempt = createAsyncThunk(
    'testAttempts/getAttempt',
    async (attemptId: string) => {
        const response = await axios.get<TestAttempt>(`/api/testattempts/${attemptId}`);
        return response.data;
    }
);

export const createTestAttempt = createAsyncThunk(
    'testAttempts/create',
    async (testId: string) => {
        const response = await axios.post<TestAttempt>('/api/testattempts', { testId });
        return response.data;
    }
);

export const completeTestAttempt = createAsyncThunk(
    'testAttempts/complete',
    async (attemptId: string) => {
        await axios.post(`/api/testattempts/${attemptId}/complete`);
        return attemptId;
    }
);

const testAttemptsSlice = createSlice({
    name: 'testAttempts',
    initialState,
    reducers: {
        clearCurrentAttempt: (state) => {
            state.currentAttempt = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(getActiveTestAttempt.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(getActiveTestAttempt.fulfilled, (state, action) => {
                state.isLoading = false;
                state.activeAttempt = action.payload;
            })
            .addCase(getActiveTestAttempt.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Failed to get active test attempt';
            })
            .addCase(getTestAttempt.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(getTestAttempt.fulfilled, (state, action) => {
                state.isLoading = false;
                state.currentAttempt = action.payload;
            })
            .addCase(getTestAttempt.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Failed to get test attempt';
            })
            .addCase(createTestAttempt.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(createTestAttempt.fulfilled, (state, action) => {
                state.isLoading = false;
                state.currentAttempt = action.payload;
                state.activeAttempt = action.payload;
            })
            .addCase(createTestAttempt.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Failed to create test attempt';
            })
            .addCase(completeTestAttempt.fulfilled, (state, action) => {
                if (state.currentAttempt?.id === action.payload) {
                    state.currentAttempt.completedAt = new Date().toISOString();
                }
                if (state.activeAttempt?.id === action.payload) {
                    state.activeAttempt = null;
                }
            });
    },
});

export const { clearCurrentAttempt } = testAttemptsSlice.actions;
export default testAttemptsSlice.reducer; 