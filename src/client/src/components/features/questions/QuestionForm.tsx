import { Button, Form, Input, InputNumber, Space, Switch } from 'antd';
import { useEffect, useState } from 'react';
import api from '../../../api/axios';
import type { Question } from '../../../types';
import { enqueueSnackbar } from 'notistack';

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

      enqueueSnackbar(`Питання ${initialValues ? 'оновлено' : 'створено'}`, { variant: 'success', autoHideDuration: 3000 });
      form.resetFields();
      onSuccess();
    } catch (error: any) {
      if (error.response?.data?.errors) {
        const validationErrors = error.response.data.errors;
        const errorMessages = validationErrors.map((err: any) => `${err.description}`).join('\n');
        enqueueSnackbar(errorMessages, { variant: 'error', autoHideDuration: 5000 });
      } else {
        enqueueSnackbar(error?.response?.data?.details || `Не вдалося ${initialValues ? 'оновити' : 'створити'} питання`, { variant: 'error', autoHideDuration: 3000 });
      }
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
        label="Текст питання"
        rules={[{ required: true, message: 'Будь ласка, введіть текст питання' }]}
      >
        <Input.TextArea rows={4} />
      </Form.Item>

      <Form.Item
        name="order"
        label="Порядок"
        rules={[{ required: true, message: 'Будь ласка, введіть порядок питання' }]}
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
                  rules={[{ required: true, message: 'Будь ласка, введіть текст відповіді' }]}
                >
                  <Input placeholder={`Відповідь ${field.name + 1}`} />
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
            {initialValues ? 'Оновити' : 'Створити'}
          </Button>
          <Button onClick={handleCancel}>
            Скасувати
          </Button>
        </Space>
      </Form.Item>
    </Form>
  );
}; 