import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { Question, PagedList } from '../../types';
import api from '../../api/axios';

interface QuestionsState {
  paged: PagedList<Question> | null;
  loading: boolean;
  error: string | null;
}

const initialState: QuestionsState = {
  paged: null,
  loading: false,
  error: null,
};

export const fetchQuestionsByTestId = createAsyncThunk(
  'questions/fetchByTestId',
  async (params: { testId: string; pageIndex?: number; pageSize?: number }) => {
    const response = await api.get<PagedList<Question>>(`/api/questions/tests/${params.testId}`, {
      params: {
        PageIndex: params.pageIndex ?? 0,
        PageSize: params.pageSize ?? 10,
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
      .addCase(fetchQuestionsByTestId.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchQuestionsByTestId.fulfilled, (state, action) => {
        state.loading = false;
        state.paged = action.payload;
      })
      .addCase(fetchQuestionsByTestId.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch questions';
      });
  },
});

export default questionsSlice.reducer; 