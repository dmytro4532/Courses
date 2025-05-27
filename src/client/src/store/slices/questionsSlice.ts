import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { PagedList, Question } from '../../types';
import api from '../../api/axios';

interface QuestionsState {
  paged: PagedList<Question> | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: QuestionsState = {
  paged: null,
  status: 'idle',
  error: null,
};

export const fetchQuestions = createAsyncThunk(
  'questions/fetchQuestions',
  async (params: { testId: string; pageIndex?: number; pageSize?: number; orderBy?: string; orderDirection?: string }) => {
    const response = await api.get<PagedList<Question>>(`/api/questions/tests/${params.testId}`, {
      params: {
        PageIndex: params.pageIndex ?? 0,
        PageSize: params.pageSize ?? 10,
        OrderBy: params.orderBy ?? 'Order',
        OrderDirection: params.orderDirection ?? 'ASC',
      },
    });
    return response.data;
  }
);

const questionsSlice = createSlice({
  name: 'questions',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchQuestions.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchQuestions.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.paged = action.payload;
      })
      .addCase(fetchQuestions.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch questions';
      });
  },
});

export default questionsSlice.reducer; 