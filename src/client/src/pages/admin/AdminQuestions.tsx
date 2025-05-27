import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Modal, Space, Table } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../store';
import { fetchQuestions } from '../../store/slices/questionsSlice';
import type { Question } from '../../types';
import { QuestionForm } from '../../components/features/questions/QuestionForm';
import { DeleteQuestionModal } from '../../components/features/questions/DeleteQuestionModal';

const AdminQuestions = () => {
  const { testId } = useParams<{ testId: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status } = useSelector((state: RootState) => state.questions);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingQuestion, setEditingQuestion] = useState<Question | null>(null);
  const [deletingQuestion, setDeletingQuestion] = useState<Question | null>(null);

  useEffect(() => {
    if (testId) {
      dispatch(fetchQuestions({ testId: testId, pageIndex: 0, pageSize: 10 }));
    }
  }, [dispatch, testId]);

  const handleAdd = () => {
    setEditingQuestion(null);
    setIsModalVisible(true);
  };

  const handleEdit = (record: Question) => {
    setEditingQuestion(record);
    setIsModalVisible(true);
  };

  const handleDelete = (record: Question) => {
    setDeletingQuestion(record);
  };

  const handleFormSuccess = () => {
    setIsModalVisible(false);
    if (testId) {
      dispatch(fetchQuestions({ testId: testId, pageIndex: 0, pageSize: 10 }));
    }
  };

  const handleDeleteSuccess = () => {
    setDeletingQuestion(null);
    if (testId) {
      dispatch(fetchQuestions({ testId: testId, pageIndex: 0, pageSize: 10 }));
    }
  };

  const columns = [
    {
      title: 'Order',
      dataIndex: 'order',
      key: 'order',
      sorter: true,
      width: 100,
    },
    {
      title: 'Question',
      dataIndex: 'content',
      key: 'content',
    },
    {
      title: 'Correct Answers',
      key: 'correctAnswers',
      render: (_: any, record: Question) => 
        record.answers.filter(a => a.isCorrect).length,
      width: 150,
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: Question) => (
        <Space>
          <Button
            type="text"
            icon={<EditOutlined />}
            onClick={() => handleEdit(record)}
          />
          <Button
            type="text"
            danger
            icon={<DeleteOutlined />}
            onClick={() => handleDelete(record)}
          />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1 style={{ margin: 0 }}>Questions</h1>
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={handleAdd}
        >
          Add Question
        </Button>
      </div>

      <Table
        columns={columns}
        dataSource={paged?.items ?? []}
        rowKey="id"
        loading={status === 'loading'}
        pagination={{
          current: (paged?.pageIndex ?? 0) + 1,
          pageSize: paged?.pageSize ?? 10,
          total: paged?.totalCount ?? 0,
          onChange: (page) => {
            if (testId) {
              dispatch(fetchQuestions({ testId: testId, pageIndex: page - 1, pageSize: 10 }));
            }
          },
        }}
      />

      <Modal
        title={editingQuestion ? 'Edit Question' : 'Add Question'}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        <QuestionForm
          testId={testId ?? ''}
          initialValues={editingQuestion ?? undefined}
          onSuccess={handleFormSuccess}
          onCancel={() => setIsModalVisible(false)}
          loading={status === 'loading'}
        />
      </Modal>

      <DeleteQuestionModal
        question={deletingQuestion}
        onClose={() => setDeletingQuestion(null)}
        onSuccess={handleDeleteSuccess}
      />
    </div>
  );
};

export default AdminQuestions; 