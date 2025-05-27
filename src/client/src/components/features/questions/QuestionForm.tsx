import { Button, Form, Input, InputNumber, Space, Switch, message } from 'antd';
import { useEffect, useState } from 'react';
import api from '../../../api/axios';
import type { Question } from '../../../types';

interface QuestionFormProps {
  testId: string;
  initialValues?: Question;
  onSuccess: () => void;
  onCancel: () => void;
  loading?: boolean;
}

interface AnswerFormData {
  value: string;
  isCorrect: boolean;
}

interface QuestionFormData {
  content: string;
  order: number;
  answers: AnswerFormData[];
}

export const QuestionForm = ({ testId, initialValues, onSuccess, onCancel, loading }: QuestionFormProps) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    form.resetFields();
  }, [form, initialValues]);

  const handleSubmit = async (values: QuestionFormData) => {
    setSubmitting(true);
    try {
      const questionData = {
        content: values.content,
        order: values.order,
        testId: testId,
        answers: values.answers.map((answer) => ({
          value: answer.value,
          isCorrect: answer.isCorrect,
        })),
      };

      if (initialValues) {
        await api.put(`/api/questions/${initialValues.id}`, {
          id: initialValues.id,
          ...questionData,
        });
      } else {
        await api.post(`/api/questions`, questionData);
      }

      message.success(`Question ${initialValues ? 'updated' : 'created'} successfully`);
      onSuccess();
    } catch (error: any) {
      message.error(error?.response?.data?.details || `Failed to ${initialValues ? 'update' : 'create'} question`);
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  const initialAnswers = initialValues?.answers.map(answer => ({
    value: answer.value,
    isCorrect: answer.isCorrect,
  })) || Array(4).fill(null).map(() => ({
    value: '',
    isCorrect: false,
  }));

  return (
    <Form
      form={form}
      layout="vertical"
      onFinish={handleSubmit}
      initialValues={{
        content: initialValues?.content || '',
        order: initialValues?.order || 0,
        answers: initialAnswers,
      }}
    >
      <Form.Item
        name="content"
        label="Question Text"
        rules={[{ required: true, message: 'Please enter the question text' }]}
      >
        <Input.TextArea rows={4} />
      </Form.Item>

      <Form.Item
        name="order"
        label="Order"
        rules={[{ required: true, message: 'Please enter the question order' }]}
      >
        <InputNumber min={0} />
      </Form.Item>

      <Form.List name="answers">
        {(fields) => (
          <>
            {fields.map((field) => (
              <div key={field.key} style={{ display: 'flex', marginBottom: 8, gap: 8 }}>
                <Form.Item
                  {...field}
                  key="answer"
                  name={[field.name, 'value']}
                  style={{ flex: 1, margin: 0 }}
                  rules={[{ required: true, message: 'Please enter answer text' }]}
                >
                  <Input placeholder={`Answer ${field.name + 1}`} />
                </Form.Item>
                <Form.Item
                  {...field}
                  key="isCorrect"
                  name={[field.name, 'isCorrect']}
                  valuePropName="checked"
                  style={{ margin: 0 }}
                >
                  <Switch />
                </Form.Item>
              </div>
            ))}
          </>
        )}
      </Form.List>

      <Form.Item style={{ marginTop: 16 }}>
        <Space>
          <Button
            type="primary"
            htmlType="submit"
            loading={submitting || loading}
          >
            {initialValues ? 'Update' : 'Create'}
          </Button>
          <Button onClick={handleCancel}>
            Cancel
          </Button>
        </Space>
      </Form.Item>
    </Form>
  );
}; 