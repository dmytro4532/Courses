import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { PagedList, TestAttempt } from '../../types';

interface TestAttemptsState {
    activeAttempt: TestAttempt | null;
    currentAttempt: TestAttempt | null;
    attempts: PagedList<TestAttempt>;
    isLoading: boolean;
    error: string | null;
}

const initialState: TestAttemptsState = {
    activeAttempt: null,
    currentAttempt: null,
    attempts: {
        pageIndex: 0,
        pageSize: 10,
        totalCount: 0,
        hasPreviousPage: false,
        hasNextPage: false,
        items: [],
    },
    isLoading: false,
    error: null,
};

export const getActiveTestAttempt = createAsyncThunk(
    'testAttempts/getActive',
    async (testId: string, { rejectWithValue }) => {
        try {
            const response = await api.get<TestAttempt>(`/api/testattempts/active/${testId}`);
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to get active test attempt');
        }
    }
);

export const getTestAttempt = createAsyncThunk(
    'testAttempts/getAttempt',
    async (attemptId: string, { rejectWithValue }) => {
        try {
            const response = await api.get<TestAttempt>(`/api/testattempts/${attemptId}`);
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to get test attempt');
        }
    }
);

export const createTestAttempt = createAsyncThunk(
    'testAttempts/create',
    async (testId: string, { rejectWithValue }) => {
        try {
            const response = await api.post<TestAttempt>('/api/testattempts', { testId });
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to create test attempt');
        }
    }
);

export const completeTestAttempt = createAsyncThunk(
    'testAttempts/complete',
    async (attemptId: string, { rejectWithValue }) => {
        try {
            await api.post(`/api/testattempts/${attemptId}/complete`, {
                testAttemptId: attemptId,
            });
            return attemptId;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to complete test attempt');
        }
    }
);

export const fetchTestAttemptsByTest = createAsyncThunk(
    'testAttempts/fetchByTest',
    async (testId: string, { rejectWithValue }) => {
        try {
            const response = await api.get<PagedList<TestAttempt>>(`/api/testattempts/test/${testId}`, {
                params: {
                  PageIndex: 0,
                  PageSize: 10,
                  OrderBy: 'Id',
                  OrderDirection: 'ASC',
                },
            });
            return response.data;
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.detail || 'Failed to fetch test attempts');
        }
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
                state.error = action.payload as string;
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
                state.error = action.payload as string;
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
                state.error = action.payload as string;
            })
            .addCase(completeTestAttempt.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(completeTestAttempt.fulfilled, (state, action) => {
                state.isLoading = false;
                if (state.currentAttempt?.id === action.payload) {
                    state.currentAttempt.completedAt = new Date().toISOString();
                }
                if (state.activeAttempt?.id === action.payload) {
                    state.activeAttempt = null;
                }
            })
            .addCase(completeTestAttempt.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload as string;
            })
            .addCase(fetchTestAttemptsByTest.pending, (state) => {
                state.isLoading = true;
                state.error = null;
            })
            .addCase(fetchTestAttemptsByTest.fulfilled, (state, action) => {
                state.isLoading = false;
                state.attempts = action.payload;
            })
            .addCase(fetchTestAttemptsByTest.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload as string;
            });
    },
});

export const { clearCurrentAttempt } = testAttemptsSlice.actions;
export default testAttemptsSlice.reducer; 