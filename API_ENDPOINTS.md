# AutismEdu.API Endpoints

## Base URL (Development)
- `http://localhost:5110`
- `https://localhost:7272`

## Authentication
للـ endpoints المحمية استخدم:
- `Authorization: Bearer <JWT_TOKEN>`

---

## Auth (`/api/Auth`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/Auth/register` | No | Register new user |
| POST | `/api/Auth/login` | No | Login |
| GET | `/api/Auth/user-info` | Yes | Current logged-in user |
| POST | `/api/Auth/change-password` | Yes | Change current user password |
| PUT | `/api/Auth/update-profile` | Yes | Update profile (form-data) |
| POST | `/api/Auth/forget-password` | No | Send OTP |
| POST | `/api/Auth/verify-otp` | No | Verify OTP |
| POST | `/api/Auth/reset-password` | No | Reset password |
| POST | `/api/Auth/logout` | Yes | Logout current user |

## Activities (`/api/Activities`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| GET | `/api/Activities` | No | Get all activities |
| POST | `/api/Activities` | No | Create activity (form-data) |
| DELETE | `/api/Activities/{id}` | No | Delete activity |

## Children (`/api/Children`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/Children` | No | Create child |
| PUT | `/api/Children/{id}` | No | Update child |
| DELETE | `/api/Children/{id}` | No | Delete child |
| GET | `/api/Children/{id}` | No | Get child by id |
| GET | `/api/Children/by-parent/{userId}` | No | Get all children for parent |

## Communication Cards (`/api/CommunicationCard`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/CommunicationCard/create` | No | Create card (form-data) |
| GET | `/api/CommunicationCard/all` | No | Get all cards |
| GET | `/api/CommunicationCard/{id}` | No | Get card by id |
| GET | `/api/CommunicationCard/category/{category}` | No | Get cards by category |
| DELETE | `/api/CommunicationCard/{id}` | No | Delete card |
| POST | `/api/CommunicationCard/assign` | No | Assign card to child |

## Child Cards (`/api/ChildCards`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| GET | `/api/ChildCards/{childId}` | No | Get cards assigned to child |

## Lessons (`/api/Lessons`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| GET | `/api/Lessons` | No | Get all lessons |
| GET | `/api/Lessons/{id}` | No | Get lesson by id |
| POST | `/api/Lessons` | No | Create lesson |
| PUT | `/api/Lessons/{id}` | No | Update lesson |
| DELETE | `/api/Lessons/{id}` | No | Delete lesson |
| POST | `/api/Lessons/{id}/tts` | No | Generate TTS for lesson |

## Performance (`/api/Performance`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/Performance` | No | Add performance record |
| GET | `/api/Performance/child/{childId}` | No | Get child performance |
| GET | `/api/Performance/child/{childId}/lesson/{lessonId}` | No | Get child performance in lesson |
| GET | `/api/Performance/child/{childId}/stats` | No | Get child performance stats |

## Reports (`/api/Reports`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| GET | `/api/Reports/child/{childId}` | No | Child report |
| GET | `/api/Reports/child/{childId}/weekly` | No | Weekly report |
| GET | `/api/Reports/child/{childId}/monthly` | No | Monthly report |

## TTS (`/api/TTS`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/TTS` | No | Generate TTS audio |

## ChatBot (`/api/ChatBot`)
| Method | Endpoint | Auth Required | Notes |
|---|---|---|---|
| POST | `/api/ChatBot/ask` | No | Ask chatbot |

---

## Notes
- `ChildActivityController` موجود لكن بدون أي actions منشورة حاليًا، لذلك لا يوجد له endpoints فعالة.
- Swagger متاح في بيئة التطوير على:
  - `/swagger`
